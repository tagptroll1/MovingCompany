import { PUBLIC_MOVINGCOMPANYAPI_URL } from "$env/static/public";

export function movingApiV1(method: string, resource: string, data?: Record<string, unknown>) {
    return fetch(`${PUBLIC_MOVINGCOMPANYAPI_URL}/api/v1/${resource}`, {
        method,
        headers: {
            'content-type': 'application/json'
        },
        body: data && JSON.stringify(data)
    });
}
