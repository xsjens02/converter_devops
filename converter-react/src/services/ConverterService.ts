export class ConverterService {
    private static apiUrl = 'http://79.76.48.213:5000/api';

    static async getEnums(): Promise<any> {
        try {
            const response = await fetch(`${this.apiUrl}/enums`, {
                method: "GET",
                headers: { "Content-Type": "application/json" }
            });

            if (!response.ok) {
                const msg = await response.text();
                throw new Error(`Error: ${msg}`);
            }

            return await response.json(); 
        } catch (error) {
            throw new Error(`Error: ${error}`);
        }
    }
    
    static async getConversion(converter:string, func:string, value:string, valueUnit?:number, secondValue?:string, secondUnit?:number, thirdUnit?:number): Promise<string> {
        try {
            const params = new URLSearchParams();

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
            
            const url = `${this.apiUrl}/${converter.toLowerCase()}s/${func.toLowerCase()}?${params.toString()}`;
            const response = await fetch(url, {
                    method: "GET",
                    headers: { "Content-Type": "application/json" }
                }
            );

            if (!response.ok) {
                const msg = await response.text();
                throw new Error(`Error: ${msg}`);
            }

            const data = await response.json();
            return data;
        } catch (error) {
            throw new Error(`Error: ${error}`);
        }
    }

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

            return await response.json();
        } catch (error) {
            throw new Error(`Error: ${error}`);
        }
    }
}