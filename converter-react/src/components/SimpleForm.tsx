import { CustomInput } from "./CustomInput";
import { CustomOption } from "./CustomOption";
import { CustomResult } from "./CustomResult";

// Define the props for the SimpleForm component
interface SimpleFormProps {
    from: string; // Selected 'from' unit
    to: string; // Selected 'to' unit
    fromValue: string; // Input value to convert
    onChangeFrom: (val: string) => void; // Handler for changing 'from' unit
    onChangeTo: (val: string) => void; // Handler for changing 'to' unit
    unitTypes: string[]; // Available unit options
    onChangeValue: (val: string) => void; // Handler for input value change
    result: string; // Conversion result
    className?: string; // Optional custom class for styling
}

// Component for rendering a simple unit conversion form
export const SimpleForm: React.FC<SimpleFormProps> = ({ 
      from, 
      to, 
      fromValue,
      onChangeFrom, 
      onChangeTo, 
      unitTypes, 
      onChangeValue, 
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
                    value={fromValue}
                    onChange={onChangeValue}
                    className="amount"
                    pattern={/^$|^\d+(?:[.]\d{0,3})?$/}
                />
            </div>
            
            <div className={`secondcolumn ${className}`}>
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