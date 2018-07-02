// ReSharper disable ArrangeMethodOrOperatorBody
// ReSharper disable InconsistentNaming

#pragma warning disable 1587

namespace AIO.Utilities
{
    using Entropy.SDK.Orbwalking;
    using Entropy.SDK.Prediction.Health;
    using Entropy.SDK.TargetSelector;

    /// <summary>
    ///     The Implementation class.
    /// </summary>
    internal static class ImplementationClass
    {
        /// <summary>
        ///     Gets the HealthPrediction implementation.
        /// </summary>
        public static IHealthPrediction IHealthPrediction => HealthPrediction.Instance;

        /// <summary>
        ///     Gets the Orbwalker implementation.
        /// </summary>
        public static IOrbwalker IOrbwalker => Orbwalker.Implementation;

        /// <summary>
        ///     Gets the TargetSelector implementation.
        /// </summary>
        public static ITargetSelector ITargetSelector => TargetSelector.Implementation;

        /// <summary>
        ///     Gets the Prediction implementation.
        /// </summary>
        public static Entropy.SDK.Prediction.Skillshots.Prediction Prediction => Entropy.SDK.Prediction.Skillshots.Prediction.Instance;
    }
}