# Vue 3 Best Practices

A concise, opinionated checklist for writing maintainable, reliable, and performant Vue 3 applications. These practices are based on the official Vue.js documentation and style guide.

## Quick checklist

- Use multi-word component names to avoid conflicts with HTML elements.
- Define detailed prop types with validation for better documentation and error prevention.
- Always use `:key` with `v-for` to maintain component state predictably.
- Avoid combining `v-if` with `v-for` on the same element; use computed properties or template wrappers.
- Use component-scoped styling (scoped CSS, CSS modules, or class-based strategies).
- Prefer PascalCase for component names in templates and kebab-case in DOM templates.
- Keep expressions in templates simple; move complex logic to computed properties or methods.
- Follow consistent component file organization and naming conventions.
- Use the Composition API for better TypeScript support and logic reusability.
- Implement proper component lifecycle management and cleanup.

---

## Component Structure and Naming

### Component Names
- Use multi-word component names except for root `App` components.
- Prefer PascalCase in Single File Components: `UserProfile.vue`, `TodoItem.vue`.
- Use kebab-case for DOM templates: `<user-profile></user-profile>`.
- Base components should have consistent prefixes: `BaseButton`, `AppIcon`, `VTable`.
- Tightly coupled child components should include parent name: `TodoList`, `TodoListItem`, `TodoListItemButton`.

### File Organization
- One component per file with meaningful filenames.
- Use consistent filename casing: either PascalCase or kebab-case throughout project.
- Order SFC sections consistently: `<script>`, `<template>`, `<style>` (or `<template>` first).
- Group related components with descriptive naming: `SearchButton`, `SearchInput`, `SearchResults`.

## Props and Data Flow

### Prop Definitions
- Always define detailed prop types, not just arrays of strings.
- Include `type`, `required`, `default`, and `validator` where appropriate.
- Use camelCase for prop names in JavaScript, kebab-case in HTML templates.
- Validate props comprehensively to catch errors early and document component APIs.

```js
// Good
const props = defineProps({
  status: {
    type: String,
    required: true,
    validator: (value) => ['pending', 'approved', 'rejected'].includes(value)
  },
  userId: {
    type: [String, Number],
    required: true
  }
})
```

### Event Handling
- Use descriptive event names following verb-noun pattern: `update:modelValue`, `item:selected`.
- Emit events for component communication rather than directly modifying props.
- Use `defineEmits()` for better TypeScript support and documentation.

## Template Best Practices

### Directives and Bindings
- Always use `:key` with `v-for` for predictable list rendering and better performance.
- Avoid `v-if` and `v-for` on the same element; use computed properties or template wrappers.
- Use directive shorthands consistently: `:` for `v-bind`, `@` for `v-on`, `#` for `v-slot`.
- Self-close components with no content in SFC: `<MyComponent />`.

### Template Organization
- Keep template expressions simple; move complex logic to computed properties.
- Use multi-line formatting for elements with multiple attributes.
- Quote all HTML attribute values for consistency and readability.
- Maintain consistent indentation and formatting.

## Composition API and Reactivity

### Composition API Patterns
- Prefer Composition API for new projects and complex logic.
- Use `ref()` for primitive values, `reactive()` for objects (but prefer `ref()` generally).
- Destructure reactive objects with `toRefs()` to maintain reactivity.
- Use `computed()` for derived state and expensive calculations.
- Implement `watch()` and `watchEffect()` for side effects and API calls.

### Lifecycle and Cleanup
- Use composition functions to organize and reuse logic.
- Clean up resources in `onUnmounted()`: event listeners, timers, subscriptions.
- Use `onErrorCaptured()` for error boundaries in parent components.
- Implement proper loading states and error handling.

## Styling and CSS

### Scoped Styling
- Use scoped CSS (`<style scoped>`) or CSS modules for component isolation.
- Avoid global styles except for app-level and layout components.
- Use consistent class naming conventions (BEM, atomic CSS, or utility-first).
- Leverage CSS custom properties for theming and consistency.

### Performance Considerations
- Use CSS-in-JS solutions judiciously; prefer traditional CSS for simpler maintenance.
- Minimize style recalculations with efficient selectors.
- Use CSS animations over JavaScript animations where possible.

## State Management and Architecture

### Local State
- Keep component state as local as possible.
- Use computed properties for derived state rather than duplicating data.
- Implement proper state normalization for complex data structures.

### Global State
- Use Pinia for complex state management needs.
- Organize stores by feature or domain, not by data type.
- Implement proper state persistence and hydration strategies.
- Use store composition for reusable state logic.

## Performance and Optimization

### Component Optimization
- Use `v-memo` sparingly for expensive list items or complex components.
- Implement virtual scrolling for large lists.
- Use `defineAsyncComponent()` for code-splitting and lazy loading.
- Minimize prop drilling with provide/inject or composables.

### Bundle Optimization
- Use dynamic imports for route-based code splitting.
- Implement proper tree-shaking by avoiding side effects in imports.
- Optimize images and assets with appropriate formats and sizes.
- Use Vue DevTools for performance profiling and debugging.

## TypeScript Integration

### Type Safety
- Use TypeScript with Vue 3 for better development experience.
- Define proper types for props, emits, and component APIs.
- Use generic components where appropriate for reusability.
- Implement proper type guards for runtime type checking.

### Development Experience
- Configure proper IDE support with Volar or Vue Language Features.
- Use strict TypeScript configuration for better error catching.
- Implement proper type inference with `defineComponent()` when needed.

## Testing and Quality

### Component Testing
- Write unit tests for component behavior, not implementation details.
- Test user interactions and edge cases thoroughly.
- Use Vue Testing Library or similar tools for user-centric testing.
- Mock external dependencies appropriately.

### Code Quality
- Use ESLint with Vue-specific rules for consistent code style.
- Implement proper error boundaries and fallback UI.
- Use Vue DevTools for debugging and performance analysis.
- Follow accessibility best practices with proper ARIA attributes.

## Security and Best Practices

### Security Considerations
- Sanitize user input and avoid `v-html` with untrusted content.
- Use CSP headers and proper CORS configuration.
- Validate data on both client and server sides.
- Implement proper authentication and authorization patterns.

### Progressive Enhancement
- Design components to work without JavaScript when possible.
- Use proper semantic HTML as foundation.
- Implement proper SEO with Vue Router and meta management.
- Consider server-side rendering for better initial load performance.

---

## References

- Vue.js Official Documentation: https://vuejs.org/
- Vue.js Style Guide: https://vuejs.org/style-guide/
- Composition API Guide: https://vuejs.org/guide/extras/composition-api-faq.html
- Vue 3 Migration Guide: https://v3-migration.vuejs.org/
- Pinia State Management: https://pinia.vuejs.org/
- Vue Testing Library: https://testing-library.com/docs/vue-testing-library/intro/
- Vue DevTools: https://devtools.vuejs.org/

This document focuses on Vue 3 specific practices. Layer framework-specific practices (Nuxt, Vite, etc.) on top of these fundamentals.