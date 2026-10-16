# S1 and S2 of TEST_SCENARIOS.md. Minimal pass: the item mod is NOT staged, the save was made with it.
# The replacement is the whole mod: it fires once per load, and failing looks like the errors it absorbs.
Feature: a save made with a mod that is now gone loads with generic items

  Background:
    Given the save "pgi-missing-mod" is loaded

  Scenario: the fixture mod is really absent
    Then mod "nelim.pgifixture.items" is not loaded
    And mod "nelim.perfectlygenericitemrenew" is loaded

  Scenario: the missing items became perfectly generic items, with no reference error
    Then a "PerfectlyGenericItem" exists
    And no errors were logged

  Scenario: the garment whose cloth is gone still loads
    Then a colonist "Doorman" exists
    And no errors were logged
