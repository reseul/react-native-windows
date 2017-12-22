namespace ReactNative.UIManager
{
    /// <summary>
    /// Partial ReactXP accessibility support: accessibilityTraits and importantForAccessibility.
    /// Remaining accessibilityLabel and accessibilityLiveRegion properties are implemented
    /// for all views in a generic way in <see cref="BaseViewManager{TFrameworkElement,TLayoutShadowNode}"/>.
    /// </summary>
    public interface IAccessible
    {
        /// <summary>
        /// ReactXP accessibilityTraits property.
        /// </summary>
        AccessibilityTrait[] AccessibilityTraits { get; set; }

        /// <summary>
        /// ReactXP importantForAccessibility property.
        /// </summary>
        ImportantForAccessibility ImportantForAccessibility { get; set; }
    }
}