-- ==========================================
-- Module: FAQ
-- Database: PAYDB
-- ==========================================

USE [PAYDB];
GO

-- ===========================================================
-- FAQ_GRADE Table - Grade/Category for FAQs
-- ===========================================================
CREATE TABLE [dbo].[FAQ_GRADE] (
    [PK] VARCHAR(255) NOT NULL PRIMARY KEY,
    [GradeName] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [SortOrder] INT NOT NULL DEFAULT 0,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] VARCHAR(255) NULL,
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] VARCHAR(255) NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [DeletedAt] DATETIME2 NULL
);
GO

CREATE INDEX [IX_FAQ_GRADE_IsActive] ON [dbo].[FAQ_GRADE] ([IsActive]);
CREATE INDEX [IX_FAQ_GRADE_SortOrder] ON [dbo].[FAQ_GRADE] ([SortOrder]);
GO

-- ===========================================================
-- FAQ_QUESTION Table - FAQ Questions
-- ===========================================================
CREATE TABLE [dbo].[FAQ_QUESTION] (
    [PK] VARCHAR(255) NOT NULL PRIMARY KEY,
    [GradeId] VARCHAR(255) NOT NULL,
    [QuestionText] NVARCHAR(MAX) NOT NULL,
    [QuestionTextAr] NVARCHAR(MAX) NULL,
    [ImageBlobUrl] NVARCHAR(MAX) NULL,
    [SortOrder] INT NOT NULL DEFAULT 0,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] VARCHAR(255) NULL,
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] VARCHAR(255) NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [DeletedAt] DATETIME2 NULL,
    CONSTRAINT [FK_FAQ_QUESTION_FAQ_GRADE] FOREIGN KEY ([GradeId]) REFERENCES [dbo].[FAQ_GRADE] ([PK])
);
GO

CREATE INDEX [IX_FAQ_QUESTION_GradeId] ON [dbo].[FAQ_QUESTION] ([GradeId]);
CREATE INDEX [IX_FAQ_QUESTION_IsActive] ON [dbo].[FAQ_QUESTION] ([IsActive]);
CREATE INDEX [IX_FAQ_QUESTION_SortOrder] ON [dbo].[FAQ_QUESTION] ([SortOrder]);
GO

-- ===========================================================
-- FAQ_ANSWER Table - FAQ Answers
-- ===========================================================
CREATE TABLE [dbo].[FAQ_ANSWER] (
    [PK] VARCHAR(255) NOT NULL PRIMARY KEY,
    [QuestionId] VARCHAR(255) NOT NULL,
    [AnswerText] NVARCHAR(MAX) NOT NULL,
    [AnswerTextAr] NVARCHAR(MAX) NULL,
    [ImageBlobUrl] NVARCHAR(MAX) NULL,
    [IsCorrect] BIT NOT NULL DEFAULT 0,
    [SortOrder] INT NOT NULL DEFAULT 0,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] VARCHAR(255) NULL,
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] VARCHAR(255) NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [DeletedAt] DATETIME2 NULL,
    CONSTRAINT [FK_FAQ_ANSWER_FAQ_QUESTION] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[FAQ_QUESTION] ([PK])
);
GO

CREATE INDEX [IX_FAQ_ANSWER_QuestionId] ON [dbo].[FAQ_ANSWER] ([QuestionId]);
CREATE INDEX [IX_FAQ_ANSWER_IsActive] ON [dbo].[FAQ_ANSWER] ([IsActive]);
CREATE INDEX [IX_FAQ_ANSWER_IsCorrect] ON [dbo].[FAQ_ANSWER] ([IsCorrect]);
CREATE INDEX [IX_FAQ_ANSWER_SortOrder] ON [dbo].[FAQ_ANSWER] ([SortOrder]);
GO

-- ===========================================================
-- Seed Data
-- ===========================================================
-- Insert sample grades
INSERT INTO [dbo].[FAQ_GRADE] ([PK], [GradeName], [Description], [SortOrder], [IsActive], [CreatedBy])
VALUES 
    ('GRADE_001', 'Grade 1', 'First Grade FAQ', 1, 1, 'SYSTEM'),
    ('GRADE_002', 'Grade 2', 'Second Grade FAQ', 2, 1, 'SYSTEM'),
    ('GRADE_003', 'Grade 3', 'Third Grade FAQ', 3, 1, 'SYSTEM');
GO

-- Insert sample questions
INSERT INTO [dbo].[FAQ_QUESTION] ([PK], [GradeId], [QuestionText], [QuestionTextAr], [SortOrder], [IsActive], [CreatedBy])
VALUES 
    ('QUES_001', 'GRADE_001', 'What is FAQ?', 'ما هي الأسئلة الشائعة؟', 1, 1, 'SYSTEM'),
    ('QUES_002', 'GRADE_001', 'How to use this system?', 'كيفية استخدام هذا النظام؟', 2, 1, 'SYSTEM'),
    ('QUES_003', 'GRADE_002', 'What are the requirements?', 'ما هي المتطلبات؟', 1, 1, 'SYSTEM');
GO

-- Insert sample answers
INSERT INTO [dbo].[FAQ_ANSWER] ([PK], [QuestionId], [AnswerText], [AnswerTextAr], [IsCorrect], [SortOrder], [IsActive], [CreatedBy])
VALUES 
    ('ANS_001', 'QUES_001', 'FAQ stands for Frequently Asked Questions', 'FAQ تعني الأسئلة الشائعة', 1, 1, 1, 'SYSTEM'),
    ('ANS_002', 'QUES_002', 'Follow the documentation and tutorials', 'اتبع التوثيق والبرامج التعليمية', 1, 1, 1, 'SYSTEM'),
    ('ANS_003', 'QUES_003', 'Check the system requirements page', 'تحقق من صفحة متطلبات النظام', 1, 1, 1, 'SYSTEM');
GO
