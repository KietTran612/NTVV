# Spec Compliance Reviewer Prompt

Use this checklist when running a spec compliance review step in single-flow mode.

**Purpose:** Verify the implementation built what was requested (nothing more, nothing less)

## Review Process

1. **Get the spec** — Read the full task requirements from the plan
2. **Get the implementation** — Read the actual code that was written
3. **Compare line by line** — Do NOT trust the implementer's report alone

## What to Check

**Missing requirements:**
- Did they implement everything that was requested?
- Are there requirements they skipped or missed?
- Did they claim something works but didn't actually implement it?

**Extra/unneeded work:**
- Did they build things that weren't requested?
- Did they over-engineer or add unnecessary features?
- Did they add "nice to haves" that weren't in spec?

**Misunderstandings:**
- Did they interpret requirements differently than intended?
- Did they solve the wrong problem?
- Did they implement the right feature but wrong way?

**Verify by reading code, not by trusting the report.**

## Report Format

- ✅ **Spec compliant** — everything matches after code inspection
- ❌ **Issues found** — list specifically what's missing or extra, with `file:line` references
