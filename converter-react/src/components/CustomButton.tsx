import React from 'react'
import './CustomButton.css'

interface ButtonProps {
    onClick: () => void;
    text: string;
    className?: string;
    disabled?: boolean;
}

const CustomButton: React.FC<ButtonProps> = ({ onClick, text, className, disabled = false }) => {
    return (
        <button className={`custom-btn ${className}`} onClick={onClick} disabled={disabled}>
            {text}
        </button>
    );
};

export default CustomButton;