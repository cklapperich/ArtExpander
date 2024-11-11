What we learned from fixing the 'Animated Cards not animating sometimes' bug:

1. The Root Issues:
- Unity was creating duplicate animator components, causing state chaos
- Unity won't start coroutines on inactive GameObjects
- Card transitions (especially to hand) temporarily make GameObjects inactive

2. Critical Fixes:
- Clean up duplicate components in both Initialize() and SetGhostCardUI
- Use `pendingStart` flag to handle initialization when GameObject is inactive
- Properly start animation when GameObject becomes active again

3. Key Code Patterns for Similar Unity Features:
- Always check GameObject.activeInHierarchy before starting coroutines
- Use pending state flags to handle deferred actions
- Clean up duplicate components proactively
- Track component identity through transitions

This is the essential knowledge we need to remember when dealing with Unity animations and component state management in the future. Would you like me to expand on any of these points?