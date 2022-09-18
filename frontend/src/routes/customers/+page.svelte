<script lang="ts">
	// fetch(`hptts://localhost:7136/api/v1/customers`).then(res => res.text).then(console.log)
	import type { PageData } from './$types';

	export let data: PageData;
	$: customers = data.customers;
</script>

<article>
	<h1>Customers</h1>
	<ul>
		{#each customers as customer}
			<li>
                {customer.id}. 
				{customer.name} 
                {#if customer.phone?.countryCode}
                    +{customer.phone?.countryCode ?? ''}
                {/if}
				{customer.phone?.phoneNumber ?? ''}
			</li>
		{/each}
	</ul>
</article>
<article>
	<h1>New customer</h1>

	<form class="new" action="/customers?/add" method="post">
		<label>
			Name
			<input name="name" aria-label="Customer name" placeholder="Name" />
		</label>
		<label>
			countrycode
			<input name="countrycode" aria-label="Phone Countrycode" placeholder="Countrycode" />
		</label>

		<label>
			phonenumber
			<input name="phonenumber" aria-label="Phonenumber" placeholder="Phonenumber" />
		</label>
		<button>Create</button>
	</form>
</article>

<style>
	article {
		width: 50%;
	}

    article ul {
        list-style: none;
    }

    article ul li {
        margin: 1rem;
    }

	article form {
		display: flex;
		flex-direction: column;
	}

    form label {
        display: flex;
        justify-content: space-between;
    }

    form button {
        height: 2rem;
        margin-top: 1rem;
    }
</style>
