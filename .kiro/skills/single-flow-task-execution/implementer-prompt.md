# Implementer Task Template

Use this template as a checklist when executing an implementation task in single-flow mode.

## Task Setup

Before starting, confirm:
- Task description is fully understood
- Dependencies and context are clear
- Acceptance criteria are defined

If anything is unclear, **ask now** before writing any code.

## Implementation Steps

1. **Understand the task** — Read the full task description from the plan
2. **Write failing tests first** (TDD) — Follow `test-driven-development` skill
3. **Implement minimal code** to make tests pass
4. **Verify** — Run tests, confirm they pass
5. **Commit** — Small, focused commit with clear message
6. **Self-review** (see below)
7. **Report back**

## Before Reporting Back: Self-Review

Review your work with fresh eyes. Ask yourself:

**Completeness:**
- Did I fully implement everything in the spec?
- Did I miss any requirements?
- Are there edge cases I didn't handle?

**Quality:**
- Is this my best work?
- Are names clear and accurate (match what things do, not how they work)?
- Is the code clean and maintainable?

**Discipline:**
- Did I avoid overbuilding (YAGNI)?
- Did I only build what was requested?
- Did I follow existing patterns in the codebase?

**Testing:**
- Do tests actually verify behavior (not just mock behavior)?
- Did I follow TDD if required?
- Are tests comprehensive?

If you find issues during self-review, fix them now before reporting.

## Report Format

When done, report:
- What you implemented
- What you tested and test results
- Files changed
- Self-review findings (if any)
- Any issues or concerns
