import { CustomInput } from "./CustomInput";
import { CustomOption } from "./CustomOption";
import { CustomResult } from "./CustomResult";

// Define the props for the PercentageForm component
interface PercentageFormProps {
    part: string; // Selected unit for the "part" value
    whole: string; // Selected unit for the "whole" value
    onChangePart: (val: string) => void; // Handler for changing the "part" unit
    onChangeWhole: (val: string) => void; // Handler for changing the "whole" unit
    unitTypes: string[]; // Array of available unit types to choose from
    partValue: string; // Numeric input value representing the "part"
    wholeValue: string; // Numeric input value representing the "whole"
    onChangePartValue: (val: string) => void; // Handler for changing the part's value
    onChangeWholeValue: (val: string) => void; // Handler for changing the whole's value
    result: string; // Calculated percentage result
    className?: string; // Optional additional class name for styling
}

// A form to calculate percentage: (part / whole) * 100
export const PercentageForm: React.FC<PercentageFormProps> = ({ 
    part,
    whole,
    onChangePart,
    onChangeWhole,
    unitTypes,
    partValue,
    wholeValue,
    onChangePartValue,
    onChangeWholeValue,
    result,
    className  
}) => {
    return (
        <div className={`form ${className}`}>
            
            <div className={`firstcolumn ${className}`}>
                <CustomOption
                    labelText="Part"
                    value={part}
                    onChange={onChangePart}
                    options={unitTypes}
                    className="partUnit"
                />
                <CustomInput
                    labelText="amount"
                    value={partValue}
                    onChange={onChangePartValue}
                    className="partAmount"
                    pattern={/^$|^\d+(?:[.]\d{0,3})?$/}
                />
            </div>
            
            <div className={`secondcolumn ${className}`}>
                <CustomOption
                    labelText="Whole"
                    value={whole}
                    onChange={onChangeWhole}
                    options={unitTypes}
                    className="wholeUnit"
                />
                <CustomInput
                    labelText="amount"
                    value={wholeValue}
                    onChange={onChangeWholeValue}
                    className="wholeAmount"
                    pattern={/^$|^\d+(?:[.]\d{0,3})?$/}
                />
                
            </div>
            <div className={`thirdcolumn ${className}`}>
                <CustomResult
                    labelText="result"
                    result={result}
                    className="calcResult"
                />
            </div>
            
        </div>
    );
};