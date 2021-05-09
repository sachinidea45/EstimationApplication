import { Customer } from "./Customer";

export class EstimationResponse {
    customer:Customer;
    goldPricePerGram:string
    weightInGram: string;
    totalPrice: string;
}
