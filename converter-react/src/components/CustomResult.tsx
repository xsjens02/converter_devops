interface CustomResultProps {
    labelText: string;
    result: string;  
    className?: string;
}

export const CustomResult: React.FC<CustomResultProps> = ({ labelText, result, className }) => {
    return (
        <div className={`custom-result ${className}`}>
            <div className={`custom-result-label ${className}`}>
                <label>{labelText}</label>
            </div>
            <div className={`custom-result-field ${className}`}>
                <input
                    type="text"
                    value={result}
                    readOnly
                    className="result-input"
                />
            </div>
        </div>
    );
};