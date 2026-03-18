-- ==========================================
-- Seed Script: SwipeTransactionService
-- InitialCreate - Sample Data
-- ==========================================
USE [SwipeTransactionDb];
GO

-- Seed sample swipe card uploads
INSERT INTO [CANTEEN_SWIPE_CARD_UPLOAD]
    ([CN_COM_COD],[CN_EMP_NUM],[CN_SWP_TIM],[CN_ITM_COD],[CN_ITM_QTN],
     [CN_BAT_DAT],[CN_BAT_NUM],[CN_SRL_NUM],[CN_ENT_DAT],[CN_CAN_NUM],
     [CN_GAT_NUM],[CN_UPD_STS])
VALUES
    (1001,'EMP001','2026-03-11 08:00:00',101,1,'2026-03-11',1,1,'2026-03-11 08:00:00','B','001','P'),
    (1001,'EMP002','2026-03-11 08:05:00',101,1,'2026-03-11',1,2,'2026-03-11 08:05:00','B','001','P'),
    (1001,'EMP003','2026-03-11 12:30:00',102,1,'2026-03-11',1,3,'2026-03-11 12:30:00','L','001','Y'),
    (1001,'EMP001','2026-03-11 12:45:00',102,1,'2026-03-11',1,4,'2026-03-11 12:45:00','L','001','Y');
GO

-- Seed CAN_DAYWISE_EMP_PUNCH
INSERT INTO [CAN_DAYWISE_EMP_PUNCH]
    ([CN_SRL_NUM],[CN_COM_COD],[CN_SYSID],[CN_CAN_NUM],[CN_PUN_DAT],[CN_TIM_IN],[CN_TIM_OUT],[CN_WRK_HRS])
VALUES
    (1, 1001, 1001, 1, '2026-03-11', '08:00:00', '17:00:00', 9.00),
    (2, 1001, 1002, 1, '2026-03-11', '08:05:00', '17:05:00', 9.00),
    (3, 1001, 1003, 1, '2026-03-11', '09:00:00', NULL, NULL);
GO

-- Seed CANTEEN_DAYWISE_AVAILED
INSERT INTO [CANTEEN_DAYWISE_AVAILED]
    ([CN_SRL_NUM],[CN_COM_COD],[CN_SYS_ID],[CN_EMP_TYP],[CN_SWP_DAT],
     [CN_ITM_COD],[CN_ITM_TYP],[CN_EE_CON],[CN_ER_CON],[CN_CAN_NUM],[CN_ITM_QTY],[CN_ENT_USR],[CN_ENT_DAT])
VALUES
    (1, 1001, 1001, 'R', '2026-03-11', 101, 'M', 15, 35, '1', 1, 9999, '2026-03-11'),
    (2, 1001, 1002, 'R', '2026-03-11', 101, 'M', 15, 35, '1', 1, 9999, '2026-03-11'),
    (3, 1001, 1001, 'R', '2026-03-11', 102, 'M', 40, 60, '1', 1, 9999, '2026-03-11');
GO
