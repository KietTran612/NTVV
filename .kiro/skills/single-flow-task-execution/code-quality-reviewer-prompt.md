# Code Quality Reviewer Prompt

Use this checklist when running a code quality review step in single-flow mode.

**Purpose:** Verify implementation is well-built (clean, tested, maintainable)

**Only proceed after spec compliance review passes.**

## Review Process

Use the `requesting-code-review` skill with these inputs:

- **WHAT_WAS_IMPLEMENTED:** [from implementer's report]
- **PLAN_OR_REQUIREMENTS:** Task N from [plan-file]
- **BASE_SHA:** [commit before task]
- **HEAD_SHA:** [current commit]
- **DESCRIPTION:** [task summary]

## What to Check

**Code quality:**
- Are names clear and descriptive?
- Is there unnecessary complexity or duplication?
- Are error cases handled properly?

**Tests:**
- Do tests verify real behavior (not just mock behavior)?
- Is coverage adequate for the feature?
- Are edge cases covered?

**Maintainability:**
- Will another developer understand this code?
- Does it follow existing patterns in the codebase?
- Is it consistent with the project's style?

## Report Format

- **Strengths** — what was done well
- **Issues** — Critical / Important / Minor, with `file:line` references
- **Assessment** — Ready to proceed / Needs fixes
