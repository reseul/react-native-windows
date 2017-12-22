namespace ReactNative.UIManager
{
    /// <summary>
    /// NOTE: This enum is organized based on priority of these traits (0 is the lowest),
    /// which can be assigned to an accessible object. On native, all traits are combined as
    /// a list. On desktop, trait with the maximum value is picked. Whenever you are adding
    /// a new trait add it in the right priority order in the list.
    ///
    /// This is copied from ReactXP definition of AccessibilityTrait enum.
    /// </summary>
    public enum AccessibilityTrait
    {
#pragma warning disable 1591 // The code is copied from somewhere else. See enum xmldoc.
        // Desktop and iOS.
        Summary,
        Adjustable,

        // Desktop, iOS, and Android.
        Button,
        Tab,
        Selected,

        // Android only.
        Radio_button_checked,
        Radio_button_unchecked,

        // iOS only.
        Link,
        Header,
        Search,
        Image,
        Plays,
        Key,
        Text,
        Disabled,
        FrequentUpdates,
        StartsMedia,
        AllowsDirectInteraction,
        PageTurn,

        // Desktop only.
        Menu,
        MenuItem,
        MenuBar,
        TabList,
        List,
        ListItem,
        ListBox,
        Group,
        CheckBox,
        Checked,
        ComboBox,
        Log,
        Status,
        Dialog,
        HasPopup,
        Option,

        // Desktop & mobile. This is at the end because this
        // is the highest priority trait.
        None
#pragma warning restore 1591
    }
}
