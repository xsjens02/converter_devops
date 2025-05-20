import { CustomInput } from "./CustomInput";
import { CustomOption } from "./CustomOption";
import { CustomResult } from "./CustomResult";
interface SimpleFormProps {
    from: string;
    to: string;
    fromValue: string;
    onChangeFrom: (val: string) => void;
    onChangeTo: (val: string) => void;
    unitTypes: string[];
    onChangeValue: (val: string) => void;
    result: string;
    className?: string;
}

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