// Service class responsible for handling API communication related to conversions
export class ConverterService {
    // Base URL for the backend API
    private static apiUrl = 'http://79.76.48.213:5000/api';

    // Fetch available enum values (units, functions, converter types) from the backend
    static async getEnums(): Promise<any> {
        try {
            const response = await fetch(`${this.apiUrl}/enums`, {
                method: "GET",
                headers: { "Content-Type": "application/json" }
            });

            // Check if the response is successful
            if (!response.ok) {
                const msg = await response.text();
                throw new Error(`Error: ${msg}`);
            }

            // Return the parsed JSON data
            return await response.json(); 
        } catch (error) {
            // Catch and rethrow any errors
            throw new Error(`Error: ${error}`);
        }
    }

    /**
     * Send a conversion request to the backend.
     * The parameters vary based on the selected function (e.g., Convert, Add, Scale).
     */
    static async getConversion(converter:string, func:string, value:string, valueUnit?:number, secondValue?:string, secondUnit?:number, thirdUnit?:number): Promise<string> {
        try {
            const params = new URLSearchParams();

            // Add query parameters depending on the selected function
            switch (func) {
                case "Convert":
                    params.set('value', value);
                    params.set('from', valueUnit!.toString());
                    params.set('to', secondUnit!.toString());
                    break;

                case "Add":
                case "Subtract":
                case "Difference":
                    params.set('a', value);
                    params.set('aUnit', valueUnit!.toString());
                    params.set('b', secondValue!);
                    params.set('bUnit', secondUnit!.toString());
                    params.set('resultUnit', thirdUnit!.toString());
                    break;

                case "Scale":
                    params.set('value', value);
                    params.set('valueUnit', valueUnit!.toString());
                    params.set('factor', secondValue!);
                    params.set('resultUnit', thirdUnit!.toString());
                    break;

                case "Percentage":
                    params.set('a', value);
                    params.set('part', valueUnit!.toString());
                    params.set('b', secondValue!);
                    params.set('whole', secondUnit!.toString());
                    break;

                default:
                    throw new Error("Unknown converter function.");
            }

            // Construct the request URL
            const url = `${this.apiUrl}/${converter.toLowerCase()}s/${func.toLowerCase()}?${params.toString()}`;
            const response = await fetch(url, {
                    method: "GET",
                    headers: { "Content-Type": "application/json" }
                }
            );

            // Check if the response is successful
            if (!response.ok) {
                const msg = await response.text();
                throw new Error(`Error: ${msg}`);
            }

            // Parse and return the conversion result
            const data = await response.json();
            return data;
        } catch (error) {
            throw new Error(`Error: ${error}`);
        }
    }

    // Fetch previously calculated conversions (memory)
    static async getMemory(): Promise<string[]> {
        try {
            const response = await fetch(`${this.apiUrl}/memory`, {
                method: "GET",
                headers: { "Content-Type": "application/json" }
            });

            if (!response.ok) {
                const msg = await response.text();
                throw new Error(`Error: ${msg}`);
            }

            // Return the memory data as an array of strings
            return await response.json();
        } catch (error) {
            throw new Error(`Error: ${error}`);
        }
    }
}