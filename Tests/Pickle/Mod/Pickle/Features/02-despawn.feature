# S4 of TEST_SCENARIOS.md. Despawn pass: config/despawn seeds PGI_remove=true, read on each rare tick
# (250 game ticks), so 600 ticks is two of them. A pass without the seed keeps the default (off), where
# the items must stay: that half is feature 01, "a PerfectlyGenericItem exists".
Feature: the despawn setting removes spawned generic items

  Scenario: with the setting on, generic items vanish on a rare tick
    Given the save "pgi-missing-mod" is loaded
    And a "PerfectlyGenericItem" exists
    When I wait 600 ticks
    Then no "PerfectlyGenericItem" exists
    And no errors were logged
