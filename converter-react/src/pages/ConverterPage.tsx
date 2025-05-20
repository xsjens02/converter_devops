import { useNavigate } from "react-router-dom";
import {useEffect, useState} from "react";
import CustomButton from "../components/CustomButton"
import { SimpleForm } from "../components/SimpleForm";
import { DualForm } from "../components/DualForm";
import { ScaleForm } from "../components/ScaleForm";
import { PercentageForm } from "../components/PercentageForm";
import { ConverterService } from "../services/ConverterService";
import { CustomOption } from "../components/CustomOption";
import type { UnitOption } from "../models/UnitOption";
import type { FunctionOption } from "../models/FunctionOption";
import type { ConverterTypeOption } from "../models/ConverterTypeOption";
import "./ConverterPage.css";

const ConverterPage = () => {
    const navigate = useNavigate();

    const [volumeUnits, setVolumeUnits] = useState<UnitOption[]>([]);
    const [weightUnits, setWeightUnits] = useState<UnitOption[]>([]);

    const [converterOptions, setConverterOptions] = useState<ConverterTypeOption[]>([]);
    const [selectedConverter, setSelectedConverter] = useState<string>("");
    const [functionOptions, setFunctionOptions] = useState<FunctionOption[]>([]);
    const [selectedFunction, setSelectedFunction] = useState<string>("");
    
    const [firstUnit, setFirstUnit] = useState<string>("");
    const [secondUnit, setSecondUnit] = useState<string>("");
    const [resultUnit, setResultUnit] = useState<string>("");
    
    const [firstInput, setFirstInput] = useState<string>("");
    const [secondInput, setSecondInput] = useState<string>("");
    
    const [result, setResult] = useState<string>("");

    const fetchEnums = async () => {
        const data = await ConverterService.getEnums();
        setVolumeUnits(data.volumeUnits);
        setWeightUnits(data.weightUnits);
        setFunctionOptions(data.converterFunctions);
        setConverterOptions(data.converterTypes);
    };

    useEffect(() => {
        fetchEnums();
    }, []);
    
    const resetAllFields = () => {
        setFirstUnit("");
        setSecondUnit("");
        setResultUnit("");
        setFirstInput("");
        setSecondInput("");
        setResult("");
    };
    

    const getIndexValue = (unitName: string): number => {
        const allUnits = selectedConverter === "Volume" ? volumeUnits : weightUnits;
        return allUnits.find(u => u.name === unitName)?.value ?? -1;
    };

    const isInputValid = (): boolean => {
        if (!selectedConverter || !selectedFunction || !firstUnit || !firstInput) return false;

        switch (selectedFunction) {
            case "Convert":
                return !!secondUnit;

            case "Add":
            case "Subtract":
            case "Difference":
                return !!secondUnit && !!secondInput && !!resultUnit;

            case "Scale":
                return !!secondInput && !!resultUnit;

            case "Percentage":
                return !!secondInput && !!secondUnit;

            default:
                return false;
        }
    };

    const handleButtonClick = async () => {
        try {
            const res = await ConverterService.getConversion(
                selectedConverter,
                selectedFunction,
                firstInput,
                getIndexValue(firstUnit),
                secondInput,
                getIndexValue(secondUnit),
                getIndexValue(resultUnit)
            );

            setResult(res);
        } catch (error) {
            console.error("Conversion error:", error);
            setResult("");
        }
    };

    const renderForm = () => {
        if (!selectedConverter) return null;  
        
        let unitTypes: string[] = [];
        switch (selectedConverter) {
            case "Volume":  unitTypes = volumeUnits.map(u => u.name); break;
            case "Weight": unitTypes = weightUnits.map(u => u.name); break;
            default: unitTypes = []; break;
        }
        
        switch (selectedFunction) {
            case "Convert":
                return (
                    <SimpleForm
                        from={firstUnit}
                        to={secondUnit}
                        fromValue={firstInput}
                        onChangeFrom={setFirstUnit}
                        onChangeTo={setSecondUnit}
                        onChangeValue={setFirstInput}
                        unitTypes={unitTypes} 
                        result={result}
                        className="simple"
                    />
                );
            case "Add":
            case "Subtract":
            case "Difference":
                return (
                    <DualForm
                        firstUnit={firstUnit}
                        secondUnit={secondUnit}
                        resultUnit={resultUnit}
                        onChangeFirstUnit={setFirstUnit}
                        onChangeSecondUnit={setSecondUnit}
                        onChangeResultUnit={setResultUnit}
                        unitTypes={unitTypes}
                        firstValue={firstInput}
                        secondValue={secondInput}
                        onChangeFirstValue={setFirstInput}
                        onChangeSecondValue={setSecondInput}
                        result={result}
                        className="dual"
                    />
                );
            case "Scale":
                return (
                    <ScaleForm
                        from={firstUnit}
                        to={resultUnit}
                        onChangeFrom={setFirstUnit}
                        onChangeTo={setResultUnit}
                        unitTypes={unitTypes}
                        value={firstInput}
                        onChangeValue={setFirstInput}
                        factor={secondInput}
                        onChangeFactor={setSecondInput}
                        result={result}
                        className="scale"
                    />
                );
            case "Percentage":
                return (
                    <PercentageForm
                        part={firstUnit}
                        whole={secondUnit}
                        onChangePart={setFirstUnit}
                        onChangeWhole={setSecondUnit}
                        unitTypes={unitTypes}
                        partValue={firstInput}
                        wholeValue={secondInput}
                        onChangePartValue={setFirstInput}
                        onChangeWholeValue={setSecondInput}
                        result={result}
                        className="percentage"
                    />
                );
            default:
                return null;
        }
    };

    return (
        <div className="converter-container">
            <CustomButton onClick={() => navigate(-1)} text="Back" className="back-btn" />
            <div className="converter-wrapper">

                {/* Converter Selection Form */}
                <div className="converter-selection">
                    <CustomOption
                        labelText="Choose a Converter"
                        value={selectedConverter}
                        onChange={(val) => {
                            setSelectedConverter(val);
                            resetAllFields();
                        }}
                        options={converterOptions.map(o => o.name)}
                        className="converter"
                    />
                    <CustomOption
                        labelText="Choose a Function"
                        value={selectedFunction}
                        onChange={(val) => {
                            setSelectedFunction(val);
                            resetAllFields();
                        }}
                        options={functionOptions.map(o => o.name)}
                        className="function"
                    />
                </div>

                {/* Convert Calculation Form */}
                {selectedConverter && selectedFunction && (
                    <div className="converter-form-wrapper">
                        <div className="converter-form">
                            {renderForm()}
                        </div>
                        <div className="converter-calc-btn">
                            <CustomButton
                                onClick={handleButtonClick}
                                text="Calculate"
                                className="calc-btn"
                                disabled={!isInputValid()}
                            />
                        </div>
                    </div>
                )}
                
            </div>
        </div>
    );
}

export default ConverterPage;