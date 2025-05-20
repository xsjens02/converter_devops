import { CustomInput } from "./CustomInput";
import { CustomOption } from "./CustomOption";
import { CustomResult } from "./CustomResult";

interface DualFormProps {
    firstUnit: string;
    secondUnit: string;
    resultUnit: string;
    onChangeFirstUnit: (val: string) => void;
    onChangeSecondUnit: (val: string) => void;
    onChangeResultUnit: (val: string) => void;
    unitTypes: string[];
    firstValue: string;
    secondValue: string;
    onChangeFirstValue: (val: string) => void;
    onChangeSecondValue: (val: string) => void;
    result: string;
    className?: string;
}

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