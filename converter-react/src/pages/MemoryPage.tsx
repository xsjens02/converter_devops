import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import CustomButton from "../components/CustomButton"
import { ConverterService } from "../services/ConverterService";
import "./MemoryPage.css";
const MemoryPage = () => {
    const navigate = useNavigate();

    // State to store the memory items
    const [memory, setMemory] = useState<string[]>([]);

    // Function to fetch memory history from the converter service
    const fetchMemory = async () => {
        try {
            const data = await ConverterService.getMemory();
            setMemory(data);
        } catch (error) {
            console.log("Error fetching memory: ", error);
        }
    }

    // Fetch memory on component mount
    useEffect(() => {
        fetchMemory();
    }, []);

    // Render the full converter memory UI
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