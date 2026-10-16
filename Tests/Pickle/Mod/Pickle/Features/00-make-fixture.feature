@requires:nelim.pickletools.savesteps
# Run once, in the make-fixture pass (the item mod is staged), never in the pass that loads the save.
# Builds the save the other features load: items of a mod, and a garment made of that mod's cloth.
Feature: make the save that holds items of a mod that is later removed

  Scenario: save a colony holding fixture items and a garment made of fixture cloth
    Given the save "test-colony" is loaded
    And a colonist "Doorman" exists
    When I spawn a "PGIFix_Widget" at (130, 150)
    And I spawn a "PGIFix_Gadget" at (131, 150)
    And I dress "Doorman" in "Apparel_Parka" made of "PGIFix_Cloth"
    Then a "PGIFix_Widget" exists
    And a "PGIFix_Gadget" exists
    And no errors were logged
    When Nelim's Pickle Tools: the game is saved as "pgi-missing-mod"
    And Nelim's Pickle Tools: the saved game "pgi-missing-mod" is handed to the mod "nelim.perfectlygenericitemrenew.pickletests"
