import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import CustomButton from "../components/CustomButton"
import { ConverterService } from "../services/ConverterService";
import "./MemoryPage.css";
const MemoryPage = () => {
    const navigate = useNavigate();

    const [memory, setMemory] = useState<string[]>([]);

    const fetchMemory = async () => {
        try {
            const data = await ConverterService.getMemory();
            setMemory(data);
        } catch (error) {
            console.log("Error fetching memory: ", error);
        }
    }

    useEffect(() => {
        fetchMemory();
    }, []);

    return (
        <div className="memory-container">
            <CustomButton onClick={() => navigate(-1)} text="Back" className="back-btn" />
            <div className="memory-wrapper">

                {/* calculator memory */}
                <div className="memory-box">
                    <h3>Converter Memory:</h3>
                    <ul>
                        {memory.length > 0 ?
                            (memory.map((item, index) => (<li key={index}>{item}</li>)))
                            : (<li></li>)}
                    </ul>
                </div>
                
            </div>
        </div>
    );
}

export default MemoryPage;