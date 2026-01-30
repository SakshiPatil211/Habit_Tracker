-- =====================================================
-- HABIT TRACKER DATABASE - COMPLETE SCHEMA
-- =====================================================
-- This is the final, complete database schema including all tables
-- and the email_notifications_enabled column for user notifications
-- =====================================================

CREATE DATABASE IF NOT EXISTS habit_tracker_db;
USE habit_tracker_db;

-- =====================================================
-- 1. USERS TABLE
-- =====================================================
CREATE TABLE IF NOT EXISTS users (
    user_id BIGINT PRIMARY KEY AUTO_INCREMENT,
    
    -- Personal Information
    first_name VARCHAR(50) NOT NULL,
    middle_name VARCHAR(50) NULL,
    last_name VARCHAR(50) NOT NULL,
    
    -- Authentication
    username VARCHAR(50) NOT NULL,
    email VARCHAR(100) NOT NULL,
    mobile_number VARCHAR(15) NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    
    -- Additional Info
    dob DATE NULL,
    
    -- Status Flags
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_mobile_verified BOOLEAN NOT NULL DEFAULT FALSE,
    is_email_verified BOOLEAN NOT NULL DEFAULT FALSE,
    
    -- Role (stored as VARCHAR, not ENUM)
    role VARCHAR(20) NOT NULL DEFAULT 'USER',  -- Values: 'USER', 'ADMIN'
    
    -- Email Notifications Preference (NEW)
    email_notifications_enabled BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- Timestamps
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    -- Unique Constraints
    CONSTRAINT uq_users_username UNIQUE (username),
    CONSTRAINT uq_users_email UNIQUE (email),
    CONSTRAINT uq_users_mobile UNIQUE (mobile_number),
    
    -- Indexes (as defined in User model)
    INDEX idx_users_username (username),
    INDEX idx_users_email (email)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================
-- 2. USER_OTP TABLE
-- =====================================================
CREATE TABLE IF NOT EXISTS user_otp (
    otp_id BIGINT PRIMARY KEY AUTO_INCREMENT,
    
    -- Foreign Key
    user_id BIGINT NOT NULL,
    
    -- OTP Data
    otp_code_hash VARCHAR(255) NOT NULL,
    
    -- OTP Type (stored as VARCHAR, not ENUM)
    otp_type VARCHAR(50) NOT NULL,  -- Values: 'FORGOT_PASSWORD', 'EMAIL_VERIFICATION'
    
    -- Channel (stored as VARCHAR, not ENUM)
    channel VARCHAR(20) NOT NULL,  -- Values: 'EMAIL', 'SMS'
    
    -- Expiration
    expires_at TIMESTAMP NOT NULL,
    
    -- Status
    is_used BOOLEAN NOT NULL DEFAULT FALSE,
    attempts INT NOT NULL DEFAULT 0,
    
    -- Timestamps
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    -- Foreign Key Constraint
    CONSTRAINT fk_user_otp_user
        FOREIGN KEY (user_id)
        REFERENCES users(user_id)
        ON DELETE CASCADE,
    
    -- Indexes (as defined in UserOtp model)
    INDEX idx_user_otp_user_id (user_id),
    INDEX idx_user_otp_expires_at (expires_at)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================
-- 3. HABIT_CATEGORIES TABLE
-- =====================================================
CREATE TABLE IF NOT EXISTS habit_categories (
    category_id BIGINT PRIMARY KEY AUTO_INCREMENT,
    
    -- Category Info
    category_name VARCHAR(100) NOT NULL,
    description VARCHAR(255) NULL,
    
    -- Status
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- Timestamps
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    -- Unique Constraint
    CONSTRAINT uq_habit_categories_name UNIQUE (category_name)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================
-- 4. HABITS TABLE
-- =====================================================
CREATE TABLE IF NOT EXISTS habits (
    habit_id BIGINT PRIMARY KEY AUTO_INCREMENT,
    
    -- Foreign Keys
    user_id BIGINT NOT NULL,
    category_id BIGINT NOT NULL,
    
    -- Habit Information
    habit_name VARCHAR(100) NOT NULL,
    description VARCHAR(255) NULL,
    
    -- Dates
    start_date DATE NOT NULL,
    end_date DATE NULL,
    
    -- Priority (stored as VARCHAR, not ENUM)
    priority VARCHAR(20) NOT NULL DEFAULT 'MEDIUM',  -- Values: 'LOW', 'MEDIUM', 'HIGH'
    
    -- Status
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- Timestamps
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    -- Foreign Key Constraints
    CONSTRAINT fk_habits_user
        FOREIGN KEY (user_id)
        REFERENCES users(user_id)
        ON DELETE CASCADE,
    
    CONSTRAINT fk_habits_category
        FOREIGN KEY (category_id)
        REFERENCES habit_categories(category_id)
        ON DELETE RESTRICT,  -- RESTRICT, not CASCADE (as per AppDbContext)
    
    -- Unique Constraint (as per AppDbContext line 113-114)
    CONSTRAINT uq_habits_user_habit UNIQUE (user_id, habit_name)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================
-- 5. HABIT_SCHEDULE TABLE
-- =====================================================
CREATE TABLE IF NOT EXISTS habit_schedule (
    schedule_id BIGINT PRIMARY KEY AUTO_INCREMENT,
    
    -- Foreign Key
    habit_id BIGINT NOT NULL,
    
    -- Day of Week (stored as VARCHAR, not ENUM)
    day_of_week VARCHAR(10) NOT NULL,  -- Values: 'MON', 'TUE', 'WED', 'THU', 'FRI', 'SAT', 'SUN'
    
    -- Foreign Key Constraint
    CONSTRAINT fk_habit_schedule_habit
        FOREIGN KEY (habit_id)
        REFERENCES habits(habit_id)
        ON DELETE CASCADE,
    
    -- Unique Constraint (as per AppDbContext line 117-118)
    CONSTRAINT uq_habit_schedule_habit_day UNIQUE (habit_id, day_of_week)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================
-- 6. HABIT_LOG TABLE
-- =====================================================
CREATE TABLE IF NOT EXISTS habit_log (
    log_id BIGINT PRIMARY KEY AUTO_INCREMENT,
    
    -- Foreign Key
    habit_id BIGINT NOT NULL,
    
    -- Log Date (DateOnly in C#)
    log_date DATE NOT NULL,
    
    -- Status (stored as VARCHAR, not ENUM)
    status VARCHAR(20) NOT NULL DEFAULT 'PENDING',  -- Values: 'PENDING', 'DONE', 'SKIPPED'
    
    -- Remarks
    remarks VARCHAR(255) NULL,
    
    -- Foreign Key Constraint
    CONSTRAINT fk_habit_log_habit
        FOREIGN KEY (habit_id)
        REFERENCES habits(habit_id)
        ON DELETE CASCADE,
    
    -- Unique Constraint (as per AppDbContext line 121-122)
    CONSTRAINT uq_habit_log_habit_date UNIQUE (habit_id, log_date)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================
-- 7. HABIT_STREAKS TABLE
-- =====================================================
CREATE TABLE IF NOT EXISTS habit_streaks (
    streak_id BIGINT PRIMARY KEY AUTO_INCREMENT,
    
    -- Foreign Key (1:1 relationship with habits)
    habit_id BIGINT NOT NULL,
    
    -- Streak Data
    current_streak INT NOT NULL DEFAULT 0,
    longest_streak INT NOT NULL DEFAULT 0,
    
    -- Last Completed Date (DateOnly? in C#)
    last_completed_date DATE NULL,
    
    -- Foreign Key Constraint
    CONSTRAINT fk_habit_streaks_habit
        FOREIGN KEY (habit_id)
        REFERENCES habits(habit_id)
        ON DELETE CASCADE,
    
    -- Unique Constraint (1:1 relationship - as per AppDbContext line 125-126)
    CONSTRAINT uq_habit_streaks_habit UNIQUE (habit_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================
-- 8. HABIT_REMINDER TABLE
-- =====================================================
CREATE TABLE IF NOT EXISTS habit_reminder (
    reminder_id BIGINT PRIMARY KEY AUTO_INCREMENT,
    
    -- Foreign Key (1:1 relationship with habits)
    habit_id BIGINT NOT NULL,
    
    -- Reminder Time (TimeSpan in C#)
    reminder_time TIME NOT NULL,
    
    -- Status
    is_enabled BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- Foreign Key Constraint
    CONSTRAINT fk_habit_reminder_habit
        FOREIGN KEY (habit_id)
        REFERENCES habits(habit_id)
        ON DELETE CASCADE,
    
    -- Unique Constraint (1:1 relationship - as per AppDbContext line 129-130)
    CONSTRAINT uq_habit_reminder_habit UNIQUE (habit_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================
-- 9. FEEDBACK TABLE
-- =====================================================
CREATE TABLE IF NOT EXISTS feedback (
    feedback_id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    
    -- Feedback Information
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL,
    rating INT NOT NULL,
    message VARCHAR(2000) NOT NULL,
    
    -- Timestamps
    created_at DATETIME(6) NOT NULL DEFAULT (CURRENT_TIMESTAMP(6))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================
-- END OF SCHEMA
-- =====================================================

-- =====================================================
-- NOTES:
-- =====================================================
-- 1. All ENUM types are stored as VARCHAR for flexibility
-- 2. All foreign keys have appropriate CASCADE/RESTRICT rules
-- 3. Unique constraints prevent duplicate data
-- 4. Indexes are created for frequently queried columns
-- 5. email_notifications_enabled column is added to users table
--    with default value TRUE (all existing and new users will
--    have notifications enabled by default)
-- =====================================================
