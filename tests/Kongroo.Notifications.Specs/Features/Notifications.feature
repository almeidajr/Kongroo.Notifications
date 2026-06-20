@webapi
Feature: Notification delivery
  The service consumes integration events and simulates sending the matching email.

Scenario: A welcome email is sent when a user is created
  When a user-created event is published for "ada@example.com" named "Ada Lovelace"
  Then a welcome email is sent to "ada@example.com"

Scenario: A confirmation email is sent when a payment is approved
  When an approved payment event is published for "grace@example.com" named "Grace Hopper"
  Then a purchase confirmation email is sent to "grace@example.com"
