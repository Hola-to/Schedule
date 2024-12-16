document.addEventListener('DOMContentLoaded', () => {
    const messages = document.querySelectorAll('.message');
    let currentIndex = 0;
    const delayBetweenMessages = 2000;
    const delayAfterLastMessage = 10000;

    function showNextMessage() {
        if (currentIndex < messages.length) {
            const message = messages[currentIndex];
            message.classList.add('visible');
            currentIndex++;
            setTimeout(showNextMessage, delayBetweenMessages);
        } else {
            setTimeout(() => {
                messages.forEach(msg => msg.classList.remove('visible'));
                currentIndex = 0;
                showNextMessage();
            }, delayAfterLastMessage);
        }
    }

    showNextMessage();
});

