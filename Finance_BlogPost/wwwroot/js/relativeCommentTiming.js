document.addEventListener("DOMContentLoaded", function () {
  // Activate the relativeTime plugin
  dayjs.extend(dayjs_plugin_relativeTime);

  // Function to update comment dates to relative time
  function updateCommentDates() {
    // Selects all elements with the comment-date class.
    const commentDateElements = document.querySelectorAll('.comment-date');
    // For each selected element, it retrieves the date from the data-date attribute, parses it with Day.js, and updates the element's text to display the relative time (e.g., "3 hours ago").
    commentDateElements.forEach(element => {
      const date = element.getAttribute('data-date');
      element.innerText = dayjs(date).fromNow();
    });
  }

  // Call the function to update dates
  updateCommentDates();
});
