import { Selector } from 'testcafe';

// Define the test fixture and set the start URL
fixture('Converter E2E tests')
    .page('http://79.76.48.213:3000');

// === Selector utilities ===

// Get a button by its label text
const getButton = (label) => Selector('button').withText(label);

// Get a dropdown by a specific class-based field name
const getDropdownOption = (name) => Selector(`.custom-option-field.${name} select`);

// Get an input field by a specific class-based field name
const getInputField = (name) => Selector(`.custom-input-field.${name} input`);

// Get a result field by name
const getResultField = (name) => Selector(`.custom-result-field.${name} input`);

// Commonly used selectors
const converterDropdown = getDropdownOption('converter');
const functionDropdown = getDropdownOption('function');
const resultField = getResultField('calcResult');

// Memory page selectors
const memoryBox = Selector('.memory-box');
const memoryBoxTitle = memoryBox.find('h3');
const memoryBoxItems = memoryBox.find('ul li');

// === Reusable function to navigate to a conversion function ===
async function navigateToFunction(t, converterType, functionType) {
    await t
        .click(getButton('Converter'))
        .click(converterDropdown)
        .click(converterDropdown.find('option').withExactText(converterType))
        .click(functionDropdown)
        .click(functionDropdown.find('option').withExactText(functionType));
}

// === Generic test function to test any conversion logic ===
async function testFunction(t, {
    converterType,
    functionType,
    unitSelections = [],
    inputValues = [],
    expectedResult
}) {
    await navigateToFunction(t, converterType, functionType);

    // Select units from dropdowns
    for (const { dropdown, value } of unitSelections) {
        const dd = getDropdownOption(dropdown);
        await t
            .click(dd)
            .click(dd.find('option').withExactText(value));
    }

    // Fill out input fields
    for (const { name, value } of inputValues) {
        await t.typeText(getInputField(name), value);
    }

    // Click calculate and assert expected result
    await t
        .click(getButton('Calculate'))
        .expect(resultField.value).eql(expectedResult);
}

// === Converter page UI and content test ===

// Validates that the converter page loads with correct dropdown options
test('Validate Converter Page', async t => {
    const expectedConverters = ['Weight', 'Volume'];
    const expectedFunctions = ['Convert', 'Add', 'Subtract', 'Scale', 'Difference', 'Percentage'];

    await t
        .expect(Selector(".home-title").innerText).eql("Choose from these two options:")
        .click(getButton("Converter"))
        .expect(converterDropdown.value).eql('')
        .expect(functionDropdown.value).eql('');

    for (const optionText of expectedConverters) {
        await t.expect(converterDropdown.find('option').withText(optionText).exists)
            .ok(`Expected converter option '${optionText}' not found`);
    }

    for (const optionText of expectedFunctions) {
        await t.expect(functionDropdown.find('option').withText(optionText).exists)
            .ok(`Expected function option '${optionText}' not found`);
    }
});

// === Functional conversion tests ===

test('Validate Convert function', async t => {
    await testFunction(t, {
        converterType: 'Volume',
        functionType: 'Convert',
        unitSelections: [
            { dropdown: 'fromUnit', value: 'Deciliter' },
            { dropdown: 'toUnit', value: 'Liter' },
        ],
        inputValues: [
            { name: 'amount', value: '1' },
        ],
        expectedResult: '0.1'
    });
});

test('Validate Add function', async t => {
    await testFunction(t, {
        converterType: 'Volume',
        functionType: 'Add',
        unitSelections: [
            { dropdown: 'firstUnit', value: 'Milliliter' },
            { dropdown: 'secondUnit', value: 'Deciliter' },
            { dropdown: 'resultUnit', value: 'Liter' },
        ],
        inputValues: [
            { name: 'firstAmount', value: '100' },
            { name: 'secondAmount', value: '1' },
        ],
        expectedResult: '0.2'
    });
});

test('Validate Subtract function', async t => {
    await testFunction(t, {
        converterType: 'Volume',
        functionType: 'Subtract',
        unitSelections: [
            { dropdown: 'firstUnit', value: 'Milliliter' },
            { dropdown: 'secondUnit', value: 'Deciliter' },
            { dropdown: 'resultUnit', value: 'Liter' },
        ],
        inputValues: [
            { name: 'firstAmount', value: '500' },
            { name: 'secondAmount', value: '4' },
        ],
        expectedResult: '0.1'
    });
});

test('Validate Scale function', async t => {
    await testFunction(t, {
        converterType: 'Volume',
        functionType: 'Scale',
        unitSelections: [
            { dropdown: 'fromUnit', value: 'Deciliter' },
            { dropdown: 'toUnit', value: 'Liter' },
        ],
        inputValues: [
            { name: 'amount', value: '1' },
            { name: 'factor', value: '10' },
        ],
        expectedResult: '1'
    });
});

test('Validate Difference function', async t => {
    await testFunction(t, {
        converterType: 'Volume',
        functionType: 'Difference',
        unitSelections: [
            { dropdown: 'firstUnit', value: 'Liter' },
            { dropdown: 'secondUnit', value: 'Deciliter' },
            { dropdown: 'resultUnit', value: 'Milliliter' },
        ],
        inputValues: [
            { name: 'firstAmount', value: '1' },
            { name: 'secondAmount', value: '15' },
        ],
        expectedResult: '500'
    });
});

test('Validate Percentage function', async t => {
    await testFunction(t, {
        converterType: 'Volume',
        functionType: 'Percentage',
        unitSelections: [
            { dropdown: 'partUnit', value: 'Deciliter' },
            { dropdown: 'wholeUnit', value: 'Liter' },
        ],
        inputValues: [
            { name: 'partAmount', value: '5' },
            { name: 'wholeAmount', value: '2' },
        ],
        expectedResult: '25'
    });
});

// === Memory page UI and content tests ===

// Validates that the Memory page shows up with required elements
test('Validate Memory Page', async t => {
    await t
        .expect(Selector(".home-title").innerText).eql("Choose from these two options:")
        .click(getButton("Memory"))
        .expect(memoryBox.exists).ok('Memory box should be visible')
        .expect(memoryBoxTitle.innerText).eql("Converter Memory:")
        .expect(memoryBoxItems.exists).ok('There should be at least one memory entry or an empty list item');
});

// Validates that the Memory box contains the expected conversion history
test('Validate Memory Box items', async t => {
    const expectedItems = [
        "percentage: 5 Deciliter / 2 Liter = 25.0000",
        "difference: 1 Liter - 15 Deciliter in Milliliter = 500.0000",
        "scale: 1 Deciliter * 10 in Liter = 1.0000",
        "subtract: 500 Milliliter - 4 Deciliter in Liter = 0.1000",
        "add: 100 Milliliter + 1 Deciliter in Liter = 0.2000",
        "convert: 1 Deciliter to Liter = 0.1000"
    ];

    await t
        .expect(Selector(".home-title").innerText).eql("Choose from these two options:")
        .click(getButton("Memory"))
        .expect(memoryBox.exists).ok('Memory box should be visible');

    // Validate each memory line matches expectations
    for (let i = 0; i < expectedItems.length -1; i++) {
        const line = await memoryBoxItems.nth(i).innerText;
        await t.expect(line).eql(expectedItems[i]);
    }
});