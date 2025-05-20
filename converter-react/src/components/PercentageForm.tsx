import { CustomInput } from "./CustomInput";
import { CustomOption } from "./CustomOption";
import { CustomResult } from "./CustomResult";
interface PercentageFormProps {
    part: string;
    whole: string;
    onChangePart: (val: string) => void; 
    onChangeWhole: (val: string) => void;
    unitTypes: string[];
    partValue: string;
    wholeValue: string;
    onChangePartValue: (val: string) => void;
    onChangeWholeValue: (val: string) => void;
    result: string;
    className?: string;
}

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