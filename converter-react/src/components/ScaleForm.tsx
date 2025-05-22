import { CustomInput } from "./CustomInput";
import { CustomOption } from "./CustomOption";
import { CustomResult } from "./CustomResult";

// Define the props expected by the ScaleForm component
interface ScaleFormProps {
    from: string; // Selected unit to scale from
    to: string; // Selected unit to scale to
    onChangeFrom: (val: string) => void; // Handler for changing the 'from' unit
    onChangeTo: (val: string) => void; // Handler for changing the 'to' unit
    unitTypes: string[]; // List of available unit options
    value: string; // Input value to be scaled
    onChangeValue: (val: string) => void; // Handler for changing the input value
    factor: string; // Scaling factor to apply
    onChangeFactor: (val: string) => void; // Handler for changing the scaling factor
    result: string; // Final scaled result
    className?: string; // Optional custom class for styling
}

// Component to render a form for scaling a value between units using a factor
export const ScaleForm: React.FC<ScaleFormProps> = ({ 
    from,
    to,
    onChangeFrom,
    onChangeTo,
    unitTypes,
    value,
    onChangeValue,
    factor,
    onChangeFactor,
    result,
    className  
}) => {
    return (
        <div className={`form ${className}`}>
            
            <div className={`firstcolumn ${className}`}>
                <CustomOption
                    labelText="From"
                    value={from}
                    onChange={onChangeFrom}
                    options={unitTypes}
                    className="fromUnit"
                />
                <CustomInput
                    labelText="amount"
                    value={value}
                    onChange={onChangeValue}
                    className="amount"
                    pattern={/^$|^\d+(?:[.]\d{0,3})?$/}
                />
            </div>
            
            <div className={`secondcolumn ${className}`}>
                <CustomInput
                    labelText="factor"
                    value={factor}
                    onChange={onChangeFactor}
                    className="factor"
                    pattern={/^\d*$/}
                />
            </div>
            
            <div className={`thirdcolumn ${className}`}>
                <CustomOption
                    labelText="To"
                    value={to}
                    onChange={onChangeTo}
                    options={unitTypes}
                    className="toUnit"
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