# Implementation details of accessibility

These are implementation details for accessibility in React Native Windows.

First iteration of implementation aims to implement accessibility as fully as possible without introducing any Windows-specific properties or specifications. The goal here is to have existing react-native-windows apps to run as-is and automatically get accessibility. It looks like this is achievable and will provide pretty good coverage for accessibility features, given that the app already has accessbility working correctly on Android and iOS.

The following properties are implemented in React Native Windows:

- `importantForAccessibilty`. Documented as Android-only. Covers functionality of `accessible` too. Expected to be specified correctly since app's accessibility is supposed to be working correctly on iOS.
- `accessibilityTraits`. Documented as iOS-only. Covers `accessibilityComponentType` too. Ability ti assign multiple traits matches Windows UAI flexibility to expose multiple control patterns on a single control.
- `accessibilityLabel`. Trivial implementation on Windows for all components.
- `accessibilityLiveRegion`. Documented as Android-only. Trivial implementation on Windows for all components.

The following properties are not implemented with the following justifications:

- `accessible`. Functionality is covered by `importantForAccessibility`. Also in the planned refactoring of accessibility there are plans to remove the property support at all.
- `accessibilityViewIsModal`. iOS-only. Has no direct or simple implementation on Windows. App's Android code path is assumed to be used on Windows in this case if needed.
- `onAccessibilityTap` and `onMagicTap`. iOS-only. iOS notion of accessibility tap and magic tap are not compatible with Windows UI Automation infrasructure. App's Android code path is assumed to be used on Windows in this case if needed.
- `accessibilityComponentType`. Android-only. Since app's iOS code path is assumed to work correctly, app should always use `accessibilityTraits` together with `accessibilityComponentType`, and we implement `accessibilityTraits`.
