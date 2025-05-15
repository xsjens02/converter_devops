interface CustomInputProps {
    labelText: string;
    value: string;
    onChange: (val: string) => void;
    className?: string;
    pattern?: RegExp;
}

export const CustomInput: React.FC<CustomInputProps> = ({ labelText, value, onChange, className, pattern }) => {
    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        const regex = pattern ?? /.*/;

        if (regex.test(value)) {
            onChange(value);
        } 
    };
    
    return (
        <div className={`custom-input ${className}`}>
            <div className={`custom-input-label ${className}`}>
                <label>{labelText}</label>
            </div>
            <div className={`custom-input-field ${className}`}>
                <input
                    type="text"
                    value={value}
                    onChange={handleChange}
                />
            </div>
        </div>
    );
};