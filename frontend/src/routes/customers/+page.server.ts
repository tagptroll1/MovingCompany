import { movingApiV1 } from "$lib/movingCompanyApi";
import type { PageServerLoad } from ".svelte-kit/types/src/routes/customers/$types";
import type { Actions } from "@sveltejs/kit";
import { error } from '@sveltejs/kit';

export const actions: Actions = {
	add: async ({ request }) => {
		const form = await request.formData();

		await movingApiV1('POST', `customers`, {
			name: form.get('name'),
            phone: {
                phoneNumber: form.get('phonenumber'),
                countryCode: form.get('countrycode')
            }
		});
	}
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

export const load: PageServerLoad = async () => {
	// locals.userid comes from src/hooks.js
    const response = await movingApiV1('GET', `customers`);
    if (response.status === 404) {
        return {
            customers: [] as Customer[]
        };
    }
    
    if (response.status === 200) {
        return {
            customers: (await response.json()) as Customer[]
        };
    }

    throw error(response.status);
};