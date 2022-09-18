<script lang="ts">
	import { page } from '$app/stores';
</script>

<header>
	<nav>
		<ul>
			<li class:active={$page.url.pathname === '/'}>
				<a href="/">Home</a>
			</li>
			<li class:active={$page.url.pathname === '/orders'}>
				<a href="/orders">Orders</a>
			</li>
			<li class:active={$page.url.pathname === '/customers'}>
				<a href="/customers">Customers</a>
			</li>
			<li class:active={$page.url.pathname === '/addresses'}>
				<a href="/addresses">Addresses</a>
			</li>
		</ul>
	</nav>
</header>

<style>
	header {
		display: flex;
		position: fixed;
		left: 0;
		top: 0;
		justify-content: space-between;

		height: 100vh;
		width: 10%;
		min-width: 8rem;
	}

	nav {
		display: flex;
		width: 100%;
		justify-content: center;
		--background: rgba(255, 255, 255, 0.7);
	}

	ul {
		width: 100%;
		position: relative;
		padding: 0;
		margin: 0;
		height: 100%;
		display: flex;
		flex-direction: column;
		align-items: center;
		list-style: none;
		background: var(--background);
	}

	li {
		position: relative;
		height: 2rem;
		width: 90%;
	}

	li.active::before {
		--size: 6px;
		content: '';
		width: 0;
		height: 0;
		position: absolute;
		top: calc(50% - var(--size));
		left: 0;
		border: var(--size) solid transparent;
		border-left: var(--size) solid var(--accent-color);
	}

	nav a {
		display: flex;
		height: 100%;
		align-items: center;
		padding: 0 1em;
		color: var(--heading-color);
		font-weight: 700;
		font-size: 0.8rem;
		text-transform: uppercase;
		letter-spacing: 0.1em;
		text-decoration: none;
		transition: color 0.2s linear;
	}

	a:hover {
		color: var(--accent-color);
	}
</style>
