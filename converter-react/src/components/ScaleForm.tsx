import { CustomInput } from "./CustomInput";
import { CustomOption } from "./CustomOption";
import { CustomResult } from "./CustomResult";
interface ScaleFormProps {
    from: string;
    to: string;
    onChangeFrom: (val: string) => void;
    onChangeTo: (val: string) => void;
    unitTypes: string[];
    value: string;
    onChangeValue: (val: string) => void;
    factor: string;
    onChangeFactor: (val: string) => void;
    result: string;
    className?: string;
}

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