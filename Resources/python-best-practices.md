# Python Best Practices

A concise, opinionated checklist for writing maintainable, readable, and Pythonic code. These practices are based on PEPs, community standards, and proven patterns from the Python ecosystem.

## Quick checklist

- Follow PEP 8 style guidelines; use automated formatters (black, autopep8).
- Write self-documenting code with clear, descriptive names.
- Use type hints for function signatures and complex data structures.
- Prefer list/dict comprehensions and generators for readability and performance.
- Handle exceptions explicitly; use specific exception types.
- Write docstrings for modules, classes, and public functions.
- Use virtual environments and pin dependencies with requirements files.
- Follow the principle of least surprise; be explicit rather than implicit.
- Write unit tests with meaningful assertions and good coverage.
- Use logging instead of print statements for debugging and monitoring.

---

## Code Style and Formatting

- Follow PEP 8 style guide: 79-character line limit, 4-space indentation, snake_case naming.
- Use automated formatters like `black` or `autopep8` for consistent formatting.
- Organize imports: standard library, third-party packages, local imports (separated by blank lines).
- Remove unused imports and variables; use tools like `isort` and `flake8`.

## Naming Conventions

- Use descriptive names: `user_count` not `n`, `calculate_tax()` not `calc()`.
- Follow PEP 8 naming: snake_case for variables/functions, PascalCase for classes, UPPER_CASE for constants.
- Use verbs for functions (`get_user`, `calculate_total`) and nouns for variables (`user_name`, `total_amount`).
- Avoid single-letter variables except for short loops and mathematical contexts.

## Type Hints and Documentation

- Use type hints for function parameters, return values, and complex variables.
- Leverage `typing` module: `List[str]`, `Dict[str, int]`, `Optional[str]`, `Union[str, int]`.
- Write clear docstrings using Google, NumPy, or Sphinx style conventions.
- Document complex algorithms, business logic, and non-obvious design decisions.

## Data Structures and Algorithms

- Use list comprehensions for simple transformations: `[x*2 for x in numbers if x > 0]`.
- Prefer generator expressions for memory efficiency with large datasets.
- Use appropriate data structures: `set` for membership testing, `collections.deque` for queues.
- Leverage built-in functions: `sum()`, `max()`, `min()`, `sorted()`, `enumerate()`, `zip()`.

## Error Handling

- Catch specific exceptions rather than bare `except:` clauses.
- Use `try`/`except`/`else`/`finally` appropriately; `else` runs when no exception occurs.
- Raise meaningful exceptions with descriptive messages: `raise ValueError(f"Invalid user ID: {user_id}")`.
- Use custom exceptions for domain-specific errors; inherit from built-in exception types.

## Functions and Classes

- Keep functions small and focused on a single responsibility.
- Use default parameters judiciously; avoid mutable defaults (`[]`, `{}`).
- Prefer composition over inheritance; use mixins and protocols for flexible design.
- Implement `__str__` and `__repr__` for custom classes; make `__repr__` unambiguous.

## File I/O and Resources

- Use context managers (`with` statements) for file operations and resource management.
- Handle encoding explicitly: `open(file, 'r', encoding='utf-8')`.
- Use `pathlib.Path` for file system operations instead of string manipulation.
- Close resources properly; prefer context managers over manual `close()` calls.

## Performance and Memory

- Profile before optimizing; use `cProfile` and `memory_profiler` for insights.
- Use generators and iterators for memory-efficient processing of large datasets.
- Leverage built-in functions and libraries (NumPy, Pandas) for performance-critical operations.
- Consider `slots` for memory-efficient classes with fixed attributes.

## Testing and Quality

- Write unit tests using `unittest`, `pytest`, or similar frameworks.
- Aim for meaningful test coverage; focus on critical paths and edge cases.
- Use mocking for external dependencies; test behavior, not implementation details.
- Use linters (`flake8`, `pylint`) and type checkers (`mypy`) in CI/CD pipelines.

## Dependencies and Environment

- Use virtual environments (`venv`, `virtualenv`, `conda`) for project isolation.
- Pin dependency versions in `requirements.txt` or `pyproject.toml`.
- Use `pip-tools` or similar for dependency management and security updates.
- Document Python version requirements and installation instructions clearly.

## Security Best Practices

- Validate and sanitize all external inputs; never trust user data.
- Use parameterized queries to prevent SQL injection attacks.
- Store secrets in environment variables or secure vaults, never in code.
- Keep dependencies updated; use tools like `safety` to check for vulnerabilities.

## Concurrency and Async

- Use `asyncio` for I/O-bound concurrent operations; avoid for CPU-bound tasks.
- Understand the GIL; use `multiprocessing` for CPU-intensive parallel work.
- Use `concurrent.futures` for simple parallel execution patterns.
- Handle async exceptions and cancellation properly in async code.

## Logging and Debugging

- Use the `logging` module instead of `print()` statements for debugging.
- Configure appropriate log levels: DEBUG, INFO, WARNING, ERROR, CRITICAL.
- Include context in log messages: user IDs, operation names, relevant data.
- Use structured logging (JSON) for production systems and log aggregation.

## Package Structure and Modules

- Organize code into logical modules and packages with clear `__init__.py` files.
- Use relative imports within packages; absolute imports for external modules.
- Follow conventional package structure: `src/`, `tests/`, `docs/`, `requirements.txt`.
- Include proper `setup.py` or `pyproject.toml` for distributable packages.

## References

- PEP 8 - Style Guide for Python Code: https://peps.python.org/pep-0008/
- PEP 257 - Docstring Conventions: https://peps.python.org/pep-0257/
- PEP 484 - Type Hints: https://peps.python.org/pep-0484/
- Google Python Style Guide: https://google.github.io/styleguide/pyguide.html
- Real Python Best Practices: https://realpython.com/tutorials/best-practices/
- The Zen of Python (PEP 20): https://peps.python.org/pep-0020/
- Python Package User Guide: https://packaging.python.org/
- Effective Python by Brett Slatkin
- Clean Code in Python by Mariano Anaya

This document focuses on core Python language practices. Layer framework-specific practices (Django, Flask, FastAPI) on top of these fundamentals.