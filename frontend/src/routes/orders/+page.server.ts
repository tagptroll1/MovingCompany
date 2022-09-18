import type { Actions } from "@sveltejs/kit";
import { movingApiV1 } from '$lib/movingCompanyApi';
import { error } from '@sveltejs/kit';
import type { PageServerLoad } from ".svelte-kit/types/src/routes/orders/$types";


export const actions: Actions = {
	add: async ({ request }) => {
		const form = await request.formData();
		const servicesSelected = form.getAll("service");

		if (servicesSelected?.length > 0) {

			let services = Number(servicesSelected.pop());
			
			for(const service of servicesSelected) {
				services |= Number(service); 
			}
			console.log(services)
		}

		// await movingApiV1('POST', `orders`, {
		// 	name: form.get('name'),
        //     phone: {
        //         phoneNumber: form.get('phonenumber'),
        //         countryCode: form.get('countrycode')
        //     }
		// });
	}
};


export const load: PageServerLoad  = async () => {
    const response = await movingApiV1('GET', `orders`);
    
    if (response.status === 404) {
        return {
            orders: [] as Order[]
        };
    }
    
    if (response.status === 200) {
        return {
            orders: (await response.json()) as Order[]
        };
    }
    throw error(response.status);
};


type Phone = {
    id: number;
    phoneNumber: string;
    countryCode: string;
}

type Customer = {
	id: number;
    name: string;
	phoneID: number;
    phone: Phone;
};

type Address = {
    id: number;
    addressline1: string;
    addressline2: string;
    addressline3: string;
    zipcode: string;
    city: string;
    country: string;
    countrycode: string;
    region: string;
}   

type Order = {
	id: number;
    customerID: number;
    customer: Customer;
    service: number;
    email: string;
    moveFromAddressID: number;
    moveFrom: Address;
    moveToAddressID: number;
    moveTo: Address;
    moveFromDate: Date;
    moveToDate: Date;
    comment: string;
    statuscode: number;
};
