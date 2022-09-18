<script lang="ts">
	// fetch(`hptts://localhost:7136/api/v1/customers`).then(res => res.text).then(console.log)
	import type { PageData } from './$types';

	export let data: PageData;
	$: orders = data.orders;
</script>

<article>
	<h1>Orders</h1>
	<ul>
		{#each orders as order}
			<li>
                {order.id}.
				{order.service}
			</li>
		{/each}
	</ul>
</article>
<article>
	<h1>New Order</h1>

	<form class="new" action="/orders?/add" method="post">
		<label>

			Select customer (downdown)
			<input name="name" aria-label="Customer name" placeholder="Name" />
		</label>
		<div>
			Service
			<section class="service">
				<label>
					<input name="service" type=checkbox aria-label="Service Moving" value=1 />
					Moving
				</label>

				<label>
					<input name="service" type=checkbox aria-label="Service Packing" value=2 />
					Packing
				</label>
				
				<label>
					<input name="service" type=checkbox aria-label="Service Cleaning" value=4 />
					Cleaning
				</label>
			</section>
		</div>


		<label>
			Email
			<input name="email" aria-label="Email for order" placeholder="Email" />
		</label>
		<label>
			Moving From (select address or fill in new)
			<input name="movefrom" aria-label="MovingFrom" placeholder="Move from" />
		</label>
		<label>
			Moving To (select address or fill in new)
			<input name="moveto" aria-label="MovingTo" placeholder="Move to" />
		</label>

		<label>
			Moving out at (select date)
			<input name="movefromdate" type="date" aria-label="Moving from date" placeholder="Moving out date" />
		</label>

		<label>
			Moving in at (select date)
			<input name="movetodate" type="date" aria-label="Moving in date" placeholder="Moving in date" />
		</label>

		<label for="comment">
			Comment
		</label>
		<textarea name="comment" id="comment" cols="30" rows="10" placeholder="Comment?"></textarea>
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

    form > label {
		margin: 1rem 0;
        display: flex;
        justify-content: space-between;
    }

	section.service {
		display: flex;
		justify-content: space-around;
	}

    form button {
        height: 2rem;
        margin-top: 1rem;
    }
</style>
