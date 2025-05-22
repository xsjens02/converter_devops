import { CustomInput } from "./CustomInput";
import { CustomOption } from "./CustomOption";
import { CustomResult } from "./CustomResult";

// Define the expected props for the DualForm component
interface DualFormProps {
    firstUnit: string;                  // Currently selected unit for the first value
    secondUnit: string;                 // Currently selected unit for the second value
    resultUnit: string;                 // Currently selected unit for the result
    onChangeFirstUnit: (val: string) => void;  // Handler to update the first unit
    onChangeSecondUnit: (val: string) => void; // Handler to update the second unit
    onChangeResultUnit: (val: string) => void; // Handler to update the result unit
    unitTypes: string[];                // Array of available units for selection
    firstValue: string;                 // Value entered for the first input
    secondValue: string;                // Value entered for the second input
    onChangeFirstValue: (val: string) => void; // Handler to update the first value
    onChangeSecondValue: (val: string) => void;// Handler to update the second value
    result: string;                    // The calculated result displayed to the user
    className?: string;                // Optional additional CSS class for styling
}

// The DualForm component renders three columns:
// 1. Input for the first value and its unit
// 2. Input for the second value and its unit
// 3. Output result unit selection and calculated result display
export const DualForm: React.FC<DualFormProps> = ({
    firstUnit, 
    secondUnit,
    resultUnit,
    onChangeFirstUnit,
    onChangeSecondUnit,
    onChangeResultUnit,
    unitTypes, 
    firstValue,
    secondValue,
    onChangeFirstValue,
    onChangeSecondValue,
    result, 
    className 
}) => {
    return (
        <div className={`form ${className}`}>
            
            <div className={`firstcolumn ${className}`}>
                <CustomOption
                    labelText="First Unit"
                    value={firstUnit}
                    onChange={onChangeFirstUnit}
                    options={unitTypes}
                    className="firstUnit"
                />
                <CustomInput
                    labelText="amount"
                    value={firstValue}
                    onChange={onChangeFirstValue}
                    className="firstAmount"
                    pattern={/^$|^\d+(?:[.]\d{0,3})?$/}
                />
            </div>
            
            <div className={`secondcolumn ${className}`}>
                <CustomOption
                    labelText="Second Unit"
                    value={secondUnit}
                    onChange={onChangeSecondUnit}
                    options={unitTypes}
                    className="secondUnit"
                />
                <CustomInput
                    labelText="amount"
                    value={secondValue}
                    onChange={onChangeSecondValue}
                    className="secondAmount"
                    pattern={/^$|^\d+(?:[.]\d{0,3})?$/}
                />
            </div>
            
            <div className={`thirdcolumn ${className}`}>
                <CustomOption
                    labelText="Result Unit"
                    value={resultUnit}
                    onChange={onChangeResultUnit}
                    options={unitTypes}
                    className="resultUnit"
                />
                <CustomResult
                    labelText="result"
                    result={result}
                    className="calcResult"
                />
            </div>
            
        </div>
    );
};