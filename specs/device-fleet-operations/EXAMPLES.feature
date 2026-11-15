# device-fleet-operations — specifications by example
# Scenario tags @w<N> map items to delivery waves (see WAVES.md / BACKLOG.md).

Feature: device-fleet-operations

  @w1 @happy-path
  Scenario: Add devices to a company
    Given a company "Acme" with 2 devices in Location 1 and Location 2
    When an AddDevicesMessage arrives for the company with 2 new devices
    Then the company has 4 locations named Location 1..Location 4
    And each new device has a generated serial number and its own location

  @w1
  Scenario: Add devices to unknown company fails
    Given no company with id 999
    When an AddDevicesMessage arrives for company 999
    Then the message processing fails and nothing is persisted

  @w1
  Scenario: Query location by id
    Given a company with a device in Location 1
    When a QueryLocationByIdMessage arrives for that location's id
    Then a result message returns the location name and its device serial

  @w1
  Scenario: Query unknown location returns nothing
    Given no location with id 999
    When a QueryLocationByIdMessage arrives for location 999
    Then a result message returns a null location

  @w2
  Scenario: API exposes fleet operations
    Given the APIs service and the message worker are running
    When POST /api/companies/{id}/devices is called with a device list
    Then the devices are added to the company
    When GET /api/locations/{id} is called for a known location
    Then the response contains the location and its device
