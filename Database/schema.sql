-- Sites API Database Schema
-- MySQL 8.0 compatible

-- Create database if it doesn't exist
CREATE DATABASE IF NOT EXISTS sitesdb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE sitesdb;

-- Create Sites table
CREATE TABLE IF NOT EXISTS Sites (
    site_id INT AUTO_INCREMENT PRIMARY KEY,
    site_name VARCHAR(255) NOT NULL,
    site_url VARCHAR(500) NOT NULL,
    site_x DOUBLE NOT NULL,
    site_y DOUBLE NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Create index on site_name for filtering performance
CREATE INDEX IF NOT EXISTS idx_sites_name ON Sites(site_name);

-- Insert sample data (optional)
INSERT IGNORE INTO Sites (site_id, site_name, site_url, site_x, site_y) VALUES
(1, 'Alpha Site', 'https://alpha.example.com', 10.5, 20.3),
(2, 'Beta Testing', 'https://beta.example.com', 15.2, 25.7),
(3, 'Gamma Ray', 'https://gamma.example.com', 12.8, 18.9),
(4, 'Alpha Beta', 'https://alphabeta.com', 22.1, 30.4),
(5, 'Production Site', 'https://prod.example.com', 8.7, 14.2),
(6, 'Test Environment', 'https://test.example.com', 19.3, 27.6),
(7, 'Development', 'https://dev.example.com', 11.9, 21.8),
(8, 'Staging Area', 'https://staging.com', 16.4, 23.1),
(9, 'Quality Assurance', 'https://qa.example.com', 13.6, 19.5),
(10, 'User Acceptance', 'https://uat.example.com', 24.7, 32.9);

-- Verify the data
SELECT COUNT(*) as total_sites FROM Sites;
SELECT * FROM Sites ORDER BY site_id LIMIT 5;
