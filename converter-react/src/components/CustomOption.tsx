interface CustomOptionProps {
    labelText: string;
    value: string;
    onChange: (val: string) => void;
    options: string[];
    className?: string;
}

export const CustomOption: React.FC<CustomOptionProps> = ({ labelText, value, onChange, options, className }) => {
    return (
        <div className={`custom-option ${className}`}>
            <div className={`custom-option-label ${className}`}>
                <span>{labelText}</span>
            </div>
            <div className={`custom-option-field ${className}`}>
                <select value={value} onChange={(e) => onChange(e.target.value)}>
                    <option value="" disabled hidden></option>
                    {options.map((option) => (
                        <option key={option} value={option}>
                            {option}
                        </option>
                    ))}
                </select>
            </div>
        </div>
    );
};