import { useNavigate } from "react-router-dom";
import CustomButton from "../components/CustomButton"
import "./HomePage.css"
const HomePage = () => {
    const navigate = useNavigate();

    return (
        <div className="home-container">
            <h1 className="home-title">Choose from these two options:</h1>
            <div className="home-routes">
                <CustomButton onClick={() => navigate('/converter')} text="Converter" className="route-btn" />
                <CustomButton onClick={() => navigate('/memory')} text="Memory" className="route-btn" />
            </div>
        </div>
    );
};
export default HomePage;