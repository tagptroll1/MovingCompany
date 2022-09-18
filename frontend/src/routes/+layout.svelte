<script lang="ts">
	import Header from '$lib/header/Header.svelte';
	import '../app.css';
</script>

<Header />

<main>
	<slot />
</main>

<footer>
	<p>Call us for help...</p>
</footer>

<style>
	main {
		display: flex;
		padding: 1rem;
		max-width: 1024px;
		margin: 0 auto;
		box-sizing: border-box;
		min-height: 100vh;
		margin-left: max(8rem, 10%)
	}

	footer {
		display: flex;
		justify-content: center;
	}
</style>
