import { Customer } from "./Customer";

export class UserResponse {
    customer:Customer
    expiration: string;
    token?: string;
}