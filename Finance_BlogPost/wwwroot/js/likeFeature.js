document.addEventListener("DOMContentLoaded", function () {
  // Get the like button element
  const btnLikeElement = document.getElementById("btnLike");
  // Get the total likes element
  const totalLikesElement = document.getElementById("totalLikes");

  // Send a GET request to the BlogPostLikeController API to get the total number of likes for the blog post
  async function getTotalLikes() {
    fetch('/api/BlogPostLike/' + blogId + '/totalLikes', {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Accept': '*/*'
      }
    }).then(data => data.json()) // Convert the response to JSON
      .then(result => totalLikesElement.innerHTML = result); // Update the total likes
  }

  // Send a POST request to the BlogPostLikeController API to add a like for the blog post
  async function addLikeForBlog() {
    fetch('/api/BlogPostLike/Add', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Accept': '*/*'
      },
      // Include the blog post ID and user ID in the request body
      body: JSON.stringify({
        blogPostId: blogId,
        userId: userId
      })
    }).then(() => {
      // Update the like button to display the filled thumbs up icon
      btnLikeElement.innerHTML = '<i class="bi bi-hand-thumbs-up-fill"></i>';
      // Update the total likes count
      getTotalLikes();
      // Toggle the like button binding of the event listeners
      btnLikeElement.removeEventListener("click", addLikeForBlog);
      btnLikeElement.addEventListener("click", removeLikeForBlog);
    });
  }

  // Send a POST request to the BlogPostLikeController API to remove a like for the blog post
  async function removeLikeForBlog() {
    fetch('/api/BlogPostLike/Remove', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Accept': '*/*'
      },
      body: JSON.stringify({
        blogPostId: blogId,
        userId: userId
      })
    }).then(() => {
      // Update the like button to display the empty thumbs up icon
      btnLikeElement.innerHTML = '<i class="bi bi-hand-thumbs-up"></i>';
      // Update the total likes count
      getTotalLikes();
      // Toggle the like button binding of the event listeners
      btnLikeElement.removeEventListener("click", removeLikeForBlog);
      btnLikeElement.addEventListener("click", addLikeForBlog);
    });
  }

  // Initialize event listener based on the initial state
  if (initialLiked === "True") {
    // Bind the removeLikeForBlog function to the like button if the blog post has already been liked
    btnLikeElement.addEventListener("click", removeLikeForBlog);
  } else {
    // Bind the addLikeForBlog function to the like button if the blog post has not been liked
    btnLikeElement.addEventListener("click", addLikeForBlog);
  }
});
