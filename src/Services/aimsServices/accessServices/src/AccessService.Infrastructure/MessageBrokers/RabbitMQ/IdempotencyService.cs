using System;
using System.Collections.Concurrent;

namespace AccessService.Infrastructure.MessageBrokers.RabbitMQ
{
    /// <summary>
    /// Service for ensuring exactly-once message processing (idempotency)
    /// Uses in-memory cache to track processed message IDs
    /// </summary>
    public class IdempotencyService
    {
        private readonly ConcurrentDictionary<string, DateTime> _processedMessages;
        private readonly TimeSpan _expiryDuration;
        private readonly System.Timers.Timer _cleanupTimer;

        public IdempotencyService(TimeSpan? expiryDuration = null)
        {
            _expiryDuration = expiryDuration ?? TimeSpan.FromHours(1);
            _processedMessages = new ConcurrentDictionary<string, DateTime>();

            // Start cleanup timer to remove expired entries
            _cleanupTimer = new System.Timers.Timer(TimeSpan.FromMinutes(10).TotalMilliseconds);
            _cleanupTimer.Elapsed += CleanupExpiredMessages;
            _cleanupTimer.Start();
        }

        /// <summary>
        /// Check if this is the first attempt to process a message
        /// Mark it as processed if it's new
        /// </summary>
        public bool IsFirstAttempt(string messageId)
        {
            if (string.IsNullOrEmpty(messageId))
            {
                throw new ArgumentNullException(nameof(messageId));
            }

            var now = DateTime.UtcNow;
            var isNewMessage = _processedMessages.TryAdd(messageId, now);

            return isNewMessage;
        }

        /// <summary>
        /// Manually mark a message as processed
        /// </summary>
        public void MarkAsProcessed(string messageId)
        {
            if (string.IsNullOrEmpty(messageId))
            {
                throw new ArgumentNullException(nameof(messageId));
            }

            _processedMessages.TryAdd(messageId, DateTime.UtcNow);
        }

        /// <summary>
        /// Check if a message has been processed
        /// </summary>
        public bool IsMessageProcessed(string messageId)
        {
            return _processedMessages.ContainsKey(messageId);
        }

        /// <summary>
        /// Remove expired message IDs from the cache
        /// </summary>
        private void CleanupExpiredMessages(object sender, System.Timers.ElapsedEventArgs e)
        {
            var now = DateTime.UtcNow;
            var expiredKeys = _processedMessages
                .Where(kvp => now - kvp.Value > _expiryDuration)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in expiredKeys)
            {
                _processedMessages.TryRemove(key, out _);
            }
        }

        public void Dispose()
        {
            _cleanupTimer?.Stop();
            _cleanupTimer?.Dispose();
            _processedMessages?.Clear();
        }
    }
}
