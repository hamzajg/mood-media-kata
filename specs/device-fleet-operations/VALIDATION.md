# Validation — device-fleet-operations

## Traceability

| AC | Criterion | Scenarios | Wave(s) | Test evidence |
|----|-----------|-----------|---------|---------------|
| 1 | Adding devices to an existing company creates one new locati | 4 | @w1 | AddDevicesToCompanyUseCaseTest.cs, MessageProcessorTest.cs |
| 2 | Adding devices to an unknown company is rejected, not silent | 4 | @w1 | AddDevicesToCompanyUseCaseTest.cs, MessageProcessorTest.cs |
| 3 | Querying a location by id returns the location with its devi | 4 | @w1 | AddDevicesToCompanyUseCaseTest.cs, MessageProcessorTest.cs |
| 4 | Querying an unknown location returns no result | 4 | @w1 | AddDevicesToCompanyUseCaseTest.cs, MessageProcessorTest.cs |
| 5 | POST /api/companies/{id}/devices publishes AddDevicesMessage | 1 | @w2 | — |

## Scenario coverage

- Scenarios: 5 total, 4 with detected test evidence, 1 without
- ✓ `Add devices to a company` (@w1)
- ✓ `Add devices to unknown company fails` (@w1)
- ✓ `Query location by id` (@w1)
- ✓ `Query unknown location returns nothing` (@w1)
- ✗ `API exposes fleet operations` (@w2)

## Demo script (manual walkthrough)

1. **Add devices to a company** — run the app, perform the scenario, observe the outcome.

## Gaps

- 1 scenario(s) lack detected test evidence — verify manually or add checks

Note: 'no evidence' means neither the scenario name nor a
token match was found in test files — verify by hand before
treating it as a real gap. '~' marks weak token-match evidence.