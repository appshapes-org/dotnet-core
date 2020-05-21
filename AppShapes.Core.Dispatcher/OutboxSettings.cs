using System.Collections.Generic;

namespace AppShapes.Core.Dispatcher
{
    public class OutboxSettings
    {
        public double CircuitBreakerDurationOfBreakInMinutes { get; set; } = 1;

        public int CircuitBreakerExceptionsAllowedBeforeBreaking { get; set; } = 2;

        public int OutboxTakeCount { get; set; } = 30;

        public List<int> RetryPolicySleepDurationsInSeconds { get; set; } = new List<int> {1, 2, 4};

        public int WaitForOutboxInSeconds { get; set; } = 1;
    }
}